import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DialogComponent } from '../dialog/dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { EditDialogComponent } from '../edit-dialog/edit-dialog.component';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})

export class TableComponent implements OnInit {
  transactions: any = [];
  dataSource: any = [];
  displayedColumns = ['id', 'categoryId', 'amount', 'currencyId', 'description', 'createdAt', 'actions'];

  constructor(private http: HttpClient, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadTransactions();
  }

  openDialog(title: string, subtitle: string, errors: string[]) {
    this.dialog.open(DialogComponent, {
      width: '400px',
      panelClass: 'error-dialog-container',
      data: { title: title, subtitle: subtitle, errors: errors }
    });
  }

  loadTransactions(): void {
    const storedDataString = sessionStorage.getItem('currentUser');
    const storedData = storedDataString ? JSON.parse(storedDataString) : null;
    const token = storedData ? storedData.token : null;

    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

    this.http.get<any>('/api/Transaction')
      .subscribe(
        response => {
          if (response.isSuccess) {
            this.transactions = response.receivedData.filter((item: any) => item.userId === userId);
            this.transactions.forEach((transaction: any) => {
              transaction.createdAt = new Date(transaction.createdAt).toLocaleDateString();
            });
            this.loadCategoriesAndCurrencies();
          }
        },
        error => {
          this.openDialog(
            error.error.message,
            "The following error(s) occured:",
            error.error.errors);
        }
      );
  }

  loadCategoriesAndCurrencies(): void {
    this.transactions.forEach((transaction: any) => {
      this.http.get<any>(`/api/Category/${transaction.categoryId}`)
        .subscribe(
          response => {
            if (response.isSuccess) {
              transaction.categoryId = `${response.receivedData.icon} ${response.receivedData.title}`;
            } else {
              console.error('Error retrieving category:', response.message);
            }
          },
          error => {
            this.openDialog(
              error.error.message,
              "The following error(s) occured:",
              error.error.errors);
          }
        );

      this.http.get<any>(`/api/Currency/${transaction.currencyId}`)
        .subscribe(
          response => {
            if (response.isSuccess) {
              transaction.currencyId = response.receivedData.code;
            } else {
              console.error('Error retrieving currency:', response.message);
            }
          },
          error => {
            this.openDialog(
              error.error.message,
              "The following error(s) occured:",
              error.error.errors);
          }
        );
    });

    this.dataSource = this.transactions;
  }

  openEditDialog(id: number) {
    this.dialog.open(EditDialogComponent, {
      width: '400px',
      data: { id: id }
    });
  }

  openDeleteDialog(row: any) {
    this.dialog.open(DeleteDialogComponent, {
      width: '400px',
      data: { row: row }
    });
  }

  editRow(row: any) {
    console.log('Edit row:', row);
    this.openEditDialog(row.id);
  }

  deleteRow(row: any) {
    console.log('Delete row:', row);
    this.openDeleteDialog(row);
  }
}

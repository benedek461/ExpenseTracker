import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { transition } from '@angular/animations';
import { AddDialogComponent } from '../add-dialog/add-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  income: string = '';
  cost: string = '';
  currentBalance: string = '';
  accounts: any = [];
  currencies: any = [];
  categories: any = [];
  transactions: any = [];
  categoryId: number = 0;
  currencyId: number = 0;
 
  constructor(private http: HttpClient, private dialog: MatDialog) {

  }

  openDialog(title: string, subtitle: string, errors: string[]) {
    this.dialog.open(DialogComponent, {
      width: '400px',
      panelClass: 'error-dialog-container',
      data: { title: title, subtitle: subtitle, errors: errors }
    });
  }

  ngOnInit(): void {
    const storedDataString = sessionStorage.getItem('currentUser');
    const storedData = storedDataString ? JSON.parse(storedDataString) : null;
    const token = storedData ? storedData.token : null;

    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

    this.getCurrenciesAndCategories();

    setTimeout(() => { }, 2000);

    this.http.get<any>(`/api/Account`)
      .subscribe(
        response => {
          this.accounts = response.receivedData.filter((item: any) => item.userId === userId);
          this.currentBalance = this.accounts[0].balance.toString() + " " + this.currencies[this.accounts[0].currencyId - 1].code;
        },
        error => {
          this.openDialog(
            error.error.message,
            "The following error(s) occured:",
            error.error.errors);
        }
    );

    this.http.get<any>(`/api/Transaction`)
      .subscribe(
        response => {
          this.transactions = response.receivedData.filter((item: any) => item.userId === userId);
          console.log(this.transactions);
          this.transactions.forEach((transaction: any) => {
            this.categoryId = transaction.categoryId;
            this.currencyId = transaction.currencyId

            if (!this.cost || this.cost.trim() === '') {
              this.cost = '0';
            }
            if (!this.income || this.income.trim() === '') {
              this.income = '0';
            }

            if (this.categories[this.categoryId - 1].type === 1) {
              

              this.cost = (parseInt(this.cost.trim()) + transaction.amount).toString() + " " + this.currencies[this.currencyId - 1].code;
            }
            else {
              this.income = (parseInt(this.income.trim()) + transaction.amount).toString() + " " + this.currencies[this.currencyId - 1].code;
            }
          });
        },
        error => {
          this.openDialog(
            error.error.message,
            "The following error(s) occured:",
            error.error.errors);
        }
      );
  }

  getCurrenciesAndCategories() {
    this.http.get<any>(`/api/Currency`)
      .subscribe(
        response => {
          this.currencies = response.receivedData;
        },
        error => {
          this.openDialog(
            error.error.message,
            "The following error(s) occured:",
            error.error.errors);
        }
      );

    this.http.get<any>(`/api/Category`)
      .subscribe(
        response => {
          this.categories = response.receivedData;
        },
        error => {
          this.openDialog(
            error.error.message,
            "The following error(s) occured:",
            error.error.errors);
        }
      );
  }

  openEditDialog() {
    this.dialog.open(AddDialogComponent, {
      width: '400px'
    });
  }

  addRow() {
    this.openEditDialog();
  }
}

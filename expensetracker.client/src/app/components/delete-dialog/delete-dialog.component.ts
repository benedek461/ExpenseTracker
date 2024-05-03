import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-delete-dialog',
  templateUrl: './delete-dialog.component.html',
  styleUrl: './delete-dialog.component.css'
})
export class DeleteDialogComponent implements OnInit {
  row: any;

  accounts: any = [];
  categories: any = [];
  myAccount: any;

  updateBalance: number = 0;

  constructor(private http: HttpClient, private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any)
  {
    this.row = data.row;
  }
    ngOnInit(): void {
      this.http.get<any>('/api/Category', {

      })
        .subscribe(
          response => {
            response.receivedData.forEach((item: any) => {
              this.categories.push(item);
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

  openDialog(title: string, subtitle: string, errors: string[]) {
    this.dialog.open(DialogComponent, {
      width: '400px',
      panelClass: 'error-dialog-container',
      data: { title: title, subtitle: subtitle, errors: errors }
    });
  }

  updateAccountBalance(balance: number, categoryType: number) { 
    this.http.get<any>('/api/Account')
      .subscribe(
        response => {
          this.accounts = response.receivedData;

          const storedDataString = sessionStorage.getItem('currentUser');
          const storedData = storedDataString ? JSON.parse(storedDataString) : null;
          const token = storedData ? storedData.token : null;

          const decodedToken = JSON.parse(atob(token.split('.')[1]));
          const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

          this.myAccount = this.accounts.filter((item: any) => item.userId === userId)[0];
          console.log(this.myAccount);
          console.log(this.myAccount.id);
          console.log("UpdateAccountBalance() balance: " + this.myAccount.balance);

          if (categoryType === 1) {
            console.log(this.updateBalance + " = " + this.myAccount.balance + " + " + balance)
            this.updateBalance = this.myAccount.balance + balance;
          }
          else if (categoryType === 0) {
            console.log(this.updateBalance + " = " + this.myAccount.balance + " - " + balance)
            this.updateBalance = this.myAccount.balance - balance;
          }

            this.http.patch<any>(`/api/Account/${this.myAccount.id}`, {
            balance: this.updateBalance
          })
            .subscribe(
              response => {

              },
              error => {
                this.openDialog(
                  error.error.message,
                  "The following error(s) occured:",
                  error.error.errors);
              }
            );
        },
        error => {
          this.openDialog(
            error.error.message,
            "The following error(s) occured:",
            error.error.errors);
        }
    );
  }

  handleDelete() {
    this.http.delete<any>(`/api/Transaction/${this.row.id}`)
      .subscribe(
        response => {
          this.openDialog(response.message,
            "Your transaction has been deleted!. Now:",
            ["Refresh to see changes!"]);
          this.handleClose();
          this.updateAccountBalance(response.receivedData.amount, this.categories[response.receivedData.categoryId].type)
        },
        error => {
          this.openDialog(
            error.error.message,
            "The following error(s) occured:",
            error.error.errors);
        }
    );
  }

  handleClose() {
    this.dialog.closeAll();
  }
}

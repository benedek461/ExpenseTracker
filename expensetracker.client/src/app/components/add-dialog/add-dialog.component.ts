import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-add-dialog',
  templateUrl: './add-dialog.component.html',
  styleUrl: './add-dialog.component.css'
})
export class AddDialogComponent implements OnInit {
  description: string = '';
  category: number = 0;
  amount: number = 0;

  userId: string = '';
  currencyId: number = 0;

  updateBalance: number = 0;

  categoryList: string[] = [];
  categoryObjects: any = [];

  accounts: any = [];
  myAccount: any;

  descriptionControl = new FormControl('', [Validators.required]);
  categoryControl = new FormControl('', [Validators.required]);
  amountControl = new FormControl('', [Validators.required]);

  categories = new FormControl('');

  errorMessageDescription = '';
  errorMessageCategory = '';
  errorMessageAmount = '';

  constructor(private http: HttpClient, private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.getUserIdAndCurrencyId();

    this.http.get<any>('/api/Category', {

    })
      .subscribe(
        response => {
          response.receivedData.forEach((item: any) => {
            this.categoryList.push(item.icon + " " + item.title);
            this.categoryObjects.push(item);
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

  updateDescriptionErrorMessage() {
    if (this.descriptionControl.hasError('required')) {
      this.errorMessageDescription = 'You must enter a value';
    } else {
      this.errorMessageDescription = '';
    }
  }

  updateAmountErrorMessage() {
    if (this.amountControl.hasError('required')) {
      this.errorMessageAmount = 'You must enter a value';
    } else {
      this.errorMessageAmount = '';
    }
  }

  openDialog(title: string, subtitle: string, errors: string[]) {
    this.dialog.open(DialogComponent, {
      width: '400px',
      panelClass: 'error-dialog-container',
      data: { title: title, subtitle: subtitle, errors: errors }
    });
  }

  handleAdd() {
    const selectedCategory = this.categories.value;

    if (selectedCategory !== null) {
      const selectedIndex = this.categoryList.indexOf(selectedCategory);
      const selectedCategoryType = this.categoryObjects[selectedIndex].type;
      console.log("CategoryType: " + selectedCategoryType);

      this.http.post<any>('/api/Transaction/', {
        userId: this.userId,
        currencyId: this.myAccount.currencyId,
        description: this.description,
        categoryId: selectedIndex + 1,
        amount: this.amount
      })
        .subscribe(
          response => {
            this.openDialog(response.message,
              "Your transaction has been added!. Now:",
              ["Refresh to see changes!"]);

            if (selectedCategoryType === 1) {
              this.updateBalance = this.myAccount.balance - this.amount;
            }
            else if (selectedCategoryType === 0) {
              this.updateBalance = this.myAccount.balance + this.amount;
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
  }

  getUserIdAndCurrencyId() {
    const storedDataString = sessionStorage.getItem('currentUser');
    const storedData = storedDataString ? JSON.parse(storedDataString) : null;
    const token = storedData ? storedData.token : null;

    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

    this.userId = userId;

    this.http.get<any>('/api/Account')
      .subscribe(
        response => {
          this.accounts = response.receivedData;

          this.myAccount = this.accounts.filter((item: any) => item.userId === userId)[0];

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

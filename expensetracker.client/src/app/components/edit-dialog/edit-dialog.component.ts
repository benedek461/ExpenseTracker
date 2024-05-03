import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { merge } from 'rxjs';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-edit-dialog',
  templateUrl: './edit-dialog.component.html',
  styleUrl: './edit-dialog.component.css'
})
export class EditDialogComponent implements OnInit {
  description: string = '';
  category: number = 0;
  amount: number = 0;
  id: number = 0;

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

  

  constructor(private http: HttpClient, private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any)
  {
    this.id = data.id;

    merge(this.descriptionControl.statusChanges, this.descriptionControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateDescriptionErrorMessage());

    merge(this.amountControl.statusChanges, this.amountControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateAmountErrorMessage());
  }
    ngOnInit(): void {
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

  updateAccountBalance(balance: number, categoryType: number) {
    this.http.get<any>('/api/Account')
      .subscribe(
        response => {
          this.accounts = response.receivedData;
          console.log(this.accounts);

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
            console.log(this.updateBalance + " = " + this.myAccount.balance + " - " + balance)
            this.updateBalance = this.myAccount.balance - balance;
          }
          else if (categoryType === 0) {
            console.log(this.updateBalance + " = " + this.myAccount.balance + " + " + balance)
            this.updateBalance = this.myAccount.balance + balance;
          }

          this.http.patch<any>(`/api/Account/${this.myAccount.id}`, {
            balance: this.updateBalance
          })
            .subscribe(
              response => {
                location.reload();
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

  handleUpdate() {
    const selectedCategory = this.categories.value;

    if (selectedCategory !== null) {
      const selectedIndex = this.categoryList.indexOf(selectedCategory);
      const selectedCategoryType = this.categoryObjects[selectedIndex].type;
      console.log("CategoryType: " + selectedCategoryType);

      this.http.patch<any>(`/api/Transaction/${this.id}`, {
        description: this.description,
        categoryId: selectedIndex + 1,
        amount: this.amount
      })
        .subscribe(
          response => {
            this.openDialog(response.message,
              "Your transaction has been updated!. Now:",
              ["Refresh to see changes!"]);
            this.dialog.closeAll();
            this.updateAccountBalance(this.amount, selectedCategoryType);
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

  handleClose() {
    this.dialog.closeAll();
  }
}

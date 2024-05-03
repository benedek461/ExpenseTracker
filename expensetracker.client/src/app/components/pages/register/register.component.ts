import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormControl, Validators } from '@angular/forms';
import { merge } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../../dialog/dialog.component';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  firstname: string = '';
  lastname: string = '';
  email: string = '';
  username: string = '';
  password: string = '';
  confirmpassword: string = '';
  accountbalance: GLfloat = 0;

  firstnameControl = new FormControl('', [Validators.required]);
  lastnameControl = new FormControl('', [Validators.required]);
  emailControl = new FormControl('', [Validators.required, Validators.email]);
  usernameControl = new FormControl('', [Validators.required]);
  passwordControl = new FormControl('', [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)[a-zA-Z0-9\W]{6,}$/)]);
  confirmpasswordControl = new FormControl('', [Validators.required]);
  accountbalanceControl = new FormControl('', [Validators.required]);
  currencies = new FormControl('');
  

  errorMessageFirstname = '';
  errorMessageLastname = '';
  errorMessageUsername = '';
  errorMessageEmail = '';
  errorMessagePassword = '';
  errorMessageConfirmpassword = '';
  errorMessageAccountbalance = '';

  currencyList: string[] = [];

  constructor(private http: HttpClient, private dialog: MatDialog, private router: Router) {
    merge(this.firstnameControl.statusChanges, this.firstnameControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateFirstnameErrorMessage());

    merge(this.lastnameControl.statusChanges, this.lastnameControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateLastnameErrorMessage());

    merge(this.usernameControl.statusChanges, this.usernameControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateUsernameErrorMessage());

    merge(this.emailControl.statusChanges, this.emailControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateEmailErrorMessage());

    merge(this.passwordControl.statusChanges, this.passwordControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updatePasswordErrorMessage());

    merge(this.confirmpasswordControl.statusChanges, this.confirmpasswordControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateConfirmpasswordErrorMessage());

    merge(this.accountbalanceControl.statusChanges, this.accountbalanceControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateAccountbalanceErrorMessage());
  }
    ngOnInit(): void {
      this.http.get<any>('/api/Currency', {

      })
        .subscribe(
          response => {
            response.receivedData.forEach((item: any) => {
              this.currencyList.push(item.code);
            });
            console.log(response.receivedData);
          },
          error => {
            this.openDialog(
              error.error.message,
              "The following error(s) occured:",
              error.error.errors);
          }
        );
    }

  updateFirstnameErrorMessage() {
    if (this.firstnameControl.hasError('required')) {
      this.errorMessageFirstname = 'You must enter a value';
    } else {
      this.errorMessageFirstname = '';
    }
  }

  updateLastnameErrorMessage() {
    if (this.lastnameControl.hasError('required')) {
      this.errorMessageLastname = 'You must enter a value';
    } else {
      this.errorMessageLastname = '';
    }
  }

  updateUsernameErrorMessage() {
    if (this.usernameControl.hasError('required')) {
      this.errorMessageUsername = 'You must enter a value';
    } else {
      this.errorMessageUsername = '';
    }
  }

  updateEmailErrorMessage() {
    if (this.emailControl.hasError('required')) {
      this.errorMessageEmail = 'You must enter a value';
    } else {
      this.errorMessageEmail = '';
    }
  }

  updatePasswordErrorMessage() {
    if (this.passwordControl.hasError('required')) {
      this.errorMessagePassword = 'You must enter a value';
    } else {
      this.errorMessagePassword = '';
    }
  }

  updateConfirmpasswordErrorMessage() {
    if (this.confirmpasswordControl.hasError('required')) {
      this.errorMessageConfirmpassword = 'You must enter a value';
    } else {
      this.errorMessageConfirmpassword = '';
    }
  }

  updateAccountbalanceErrorMessage() {
    if (this.accountbalanceControl.hasError('required')) {
      this.errorMessageAccountbalance = 'You must enter a value';
    } else {
      this.errorMessageAccountbalance = '';
    }
  }

  openDialog(title: string, subtitle: string ,errors: string[]) {
    this.dialog.open(DialogComponent, {
      width: '400px',
      panelClass: 'error-dialog-container',
      data: { title: title, subtitle: subtitle, errors: errors }
    });
  }

  handleLogin() {
    this.router.navigate(['/login']);
  }

  handleAccountCreation(userId: string) {
    const selectedCurrency = this.currencies.value;
    console.log('Selected currency:', selectedCurrency);

    if (selectedCurrency !== null) {
      const selectedIndex = this.currencyList.indexOf(selectedCurrency);
      console.log('Selected currency index:', selectedIndex);

      this.http.post<any>('/api/Account', {
        userId: userId,
        currencyId: selectedIndex + 1,
        balance: this.accountbalance,
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
    }
  }

  handleRegister() {
    this.http.post<any>('/api/User', {
      firstName: this.firstname,
      lastName: this.lastname,
      userName: this.username,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmpassword
    })
      .subscribe(
        response => {
          console.log(response.message);
          this.openDialog(response.message,
            "Your user account has been created. Now:",
            ["Check your e - mail inbox for a confirmation link!"]);
          this.handleAccountCreation(response.receivedData.id);
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

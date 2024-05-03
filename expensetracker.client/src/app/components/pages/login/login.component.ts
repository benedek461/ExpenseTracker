import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormControl, Validators } from '@angular/forms';
import { merge } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../../dialog/dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  usernameControl = new FormControl('', [Validators.required]);
  passwordControl = new FormControl('', [Validators.required]);

  errorMessageUsername = '';
  errorMessagePassword = '';

  constructor(private http: HttpClient, private dialog: MatDialog, private router: Router) {
    merge(this.usernameControl.statusChanges, this.usernameControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updateUsernameErrorMessage());

    merge(this.passwordControl.statusChanges, this.passwordControl.valueChanges)
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.updatePasswordErrorMessage());
  }

  updateUsernameErrorMessage() {
    if (this.usernameControl.hasError('required')) {
      this.errorMessageUsername = 'You must enter a value';
    } else {
      this.errorMessageUsername = '';
    }
  }

  updatePasswordErrorMessage() {
    if (this.passwordControl.hasError('required')) {
      this.errorMessagePassword = 'You must enter a value';
    } else {
      this.errorMessagePassword = '';
    }
  }

  openDialog(title: string, subtitle: string, errors: string[]) {
    this.dialog.open(DialogComponent, {
      width: '400px',
      panelClass: 'error-dialog-container',
      data: { title: title, subtitle: subtitle ,errors: errors }
    });
  }

  handleLogin() {
    this.http.post<any>('/api/Auth/Login', { userName: this.username, password: this.password })
      .subscribe(
        response => {
          this.router.navigate(['/dashboard']);

          sessionStorage.setItem('currentUser', JSON.stringify(response.receivedData));
        },
        error => {
          this.openDialog(error.error.message,"The following error(s) occured:" ,error.error.errors);
        }
      );
  }

  handleRegister() {
    this.router.navigate(['/register']);
  }
}

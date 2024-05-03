import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent {
  firstname: string = '';
  lastname: string = '';
  username: string = '';

  constructor(private http: HttpClient, private dialog: MatDialog) {

  }

  openDialog(title: string, subtitle: string, errors: string[]) {
    this.dialog.open(DialogComponent, {
      width: '400px',
      panelClass: 'error-dialog-container',
      data: { title: title, subtitle: subtitle, errors: errors }
    });
  }

  handleUpdate() {
    const storedDataString = sessionStorage.getItem('currentUser');
    const storedData = storedDataString ? JSON.parse(storedDataString) : null;
    const token = storedData ? storedData.token : null;

    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    const userName = decodedToken['UserName'];
    const lastName = decodedToken['LastName'];
    const firstName = decodedToken['FirstName'];

    const updatedData = {
      firstname: this.firstname !== null ? this.firstname : firstName,
      lastname: this.lastname !== null ? this.lastname : lastName,
      username: this.username !== null ? this.username : userName
    };

    this.http.patch<any>(`/api/User?id=${userId}`, updatedData)
      .subscribe(
        response => {
          this.openDialog(response.message,
            "Your user account has been updated!. Now:",
            ["Relogin to see changes!"]);
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


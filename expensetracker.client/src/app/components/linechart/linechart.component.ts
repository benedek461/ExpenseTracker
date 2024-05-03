import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { DialogComponent } from '../dialog/dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-linechart',
  templateUrl: './linechart.component.html',
  styleUrl: './linechart.component.css'
})
export class LinechartComponent implements OnInit {
  dataIncome: any = [];
  dataCost: any = [];
  dataCreateAt: any = [];

  transactions: any = [];
  categories: any = [];
  categoryId: number = 0;
  lineChart: Chart | undefined;

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

    this.getCategories();

    setTimeout(() => 5000);

    this.http.get<any>('api/Transaction')
      .subscribe(
        response => {
          this.transactions = response.receivedData.filter((item: any) => item.userId === userId);;
          this.transactions.forEach((transaction: any) => {
            console.log(this.categoryId);
            this.categoryId = transaction.categoryId;
            if (this.categories[this.categoryId - 1].type === 1) {
              this.dataCost.push(transaction.amount);
            }
            else {
              this.dataIncome.push(transaction.amount);
            }
            this.dataCreateAt.push(transaction.createdAt = new Date(transaction.createdAt).toLocaleDateString());
          });

          this.initLineChart();
        },
        error => {
          this.openDialog(
            error.error.message,
            "The following error(s) occured:",
            error.error.errors);
        }
    )
  }

  getCategories() {
    this.http.get<any>('api/Category')
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

  initLineChart(): void {
    this.lineChart = new Chart({
      chart: {
        type: 'line'
      },
      title: {
        text: 'Income/Cost'
      },
      credits: {
        enabled: false
      },
      xAxis: {
        categories: this.dataCreateAt
      },
      series: [
        {
          name: 'Income',
          data: this.dataIncome
        } as any,
        {
          name: 'Cost',
          data: this.dataCost
        } as any,
      ]
    });
  }
}

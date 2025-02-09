import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface DriverStanding {
  position: number;
  driver: string;
  nationality: string;
  car: string;
  points: number;
}
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent implements OnInit {
  title = 'Frontend';
  standings: DriverStanding[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get<DriverStanding[]>('https://localhost:7131/api/f1/standings/2024')
      .subscribe(data => this.standings = data);
  }
}

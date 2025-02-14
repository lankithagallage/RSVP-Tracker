import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EventRepository } from '../../core/repositories/event.repository';
import { EventDto } from '../../core/api/api-client';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  upcomingEvents: EventDto[] = [];

  constructor(private repository: EventRepository) {}

  ngOnInit(): void {
    this.repository.fetchEvents(1, 2).subscribe((data) => {
      this.upcomingEvents = data;
    });
  }
}

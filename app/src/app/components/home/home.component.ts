import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EventStateService, Event } from '../../services/event-state.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  upcomingEvents: Event[] = [];

  constructor(private eventState: EventStateService) {}

  ngOnInit(): void {
    this.eventState.getEvents().subscribe((data) => {
      this.upcomingEvents = data.slice(0, 3);
    });
  }
}

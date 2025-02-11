import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EventStateService, Event } from '../../services/event-state.service';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss'],
})
export class EventsComponent implements OnInit {
  events: Event[] = [];

  constructor(private eventState: EventStateService) {}

  ngOnInit(): void {
    this.eventState.getEvents().subscribe((data) => {
      this.events = data.slice(0, 6);
    });
  }
}

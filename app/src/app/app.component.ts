import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { EventApiService } from './services/event-api.service';
import { EventStateService } from './services/event-state.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(
    private eventApi: EventApiService,
    private eventState: EventStateService
  ) {}

  ngOnInit(): void {
    this.eventApi.fetchEvents().subscribe((events) => {
      this.eventState.setEvents(events);
    });
  }
}

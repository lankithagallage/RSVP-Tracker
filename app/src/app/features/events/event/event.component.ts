import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Observable } from 'rxjs';
import { EventItemDto } from '../../../core/api/api-client';
import { EventRepository } from '../../../core/repositories/event.repository';
import { RsvpModalComponent } from '../rsvp-modal/rsvp-model.component';

@Component({
  selector: 'app-event',
  standalone: true,
  imports: [RouterModule, CommonModule, RsvpModalComponent],
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss'],
})
export class EventComponent implements OnInit {
  eventId: string | null = null;
  event$!: Observable<EventItemDto>;
  showModal = false;

  constructor(
    private repository: EventRepository,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.eventId = this.route.snapshot.paramMap.get('id');
    if (this.eventId) {
      this.loadEvent(this.eventId);
    }
  }

  loadEvent(eventId: string): void {
    this.event$ = this.repository.events(eventId);
  }

  openModal(eventId: string): void {
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onRsvpSuccess(): void {
    this.showModal = false;
    if (this.eventId) {
      this.loadEvent(this.eventId);
    }
  }
}

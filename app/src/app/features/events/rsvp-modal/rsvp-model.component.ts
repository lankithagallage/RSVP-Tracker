import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RsvpRepository } from '../../../core/repositories/rsvp.repository';
import { SaveRsvpRequest } from '../../../core/api/api-client';

@Component({
  selector: 'app-rsvp-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './rsvp-model.component.html',
  styleUrls: ['./rsvp-model.component.scss'],
})
export class RsvpModalComponent {
  @Input() eventId!: string; // Passed from parent component
  @Output() closeModal = new EventEmitter<void>();
  @Output() rsvpSuccess = new EventEmitter<void>();

  firstName = '';
  lastName = '';
  email = '';
  loading = false;
  errorMessage = '';

  constructor(private rsvpRepository: RsvpRepository) {}

  submitRsvp(): void {
    if (!this.firstName || !this.lastName || !this.email) {
      this.errorMessage = 'All fields are required.';
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const request = new SaveRsvpRequest();
    request.firstName = this.firstName;
    request.lastName = this.lastName;
    request.email = this.email;

    this.rsvpRepository.submitRsvp(this.eventId, request).subscribe({
      next: () => {
        this.loading = false;
        this.rsvpSuccess.emit(); // Notify parent
        this.closeModal.emit(); // Close modal
      },
      error: (err) => {
        this.loading = false;
        this.errorMessage = err || 'RSVP failed. Please try again.';
        console.error('RSVP Error:', err);
      },
    });
  }
}

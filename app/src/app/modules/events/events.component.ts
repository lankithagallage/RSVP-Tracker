import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { EventRepository } from '../../repository/event.repository';
import { EventDtoListPagedResult } from '../../services/api-client';

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss'],
})
export class EventsComponent implements OnInit {
  pagedObject: EventDtoListPagedResult = new EventDtoListPagedResult();
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 6;
  sortBy: string = 'date';
  sortOrder: string = 'desc';

  constructor(
    private repository: EventRepository,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.searchTerm = params['search'] || '';
      this.currentPage = params['page'] ? parseInt(params['page'], 10) : 1;
      this.sortBy = params['sort'] || 'date';
      this.sortOrder = params['order'] || 'desc';

      this.loadEvents();
    });
  }

  loadEvents(): void {
    this.updateUrl();

    this.repository
      .searchEvents(
        this.searchTerm,
        this.currentPage,
        this.pageSize,
        this.sortBy,
        this.sortOrder
      )
      .subscribe((data) => {
        this.pagedObject = data || new EventDtoListPagedResult();
      });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadEvents();
  }

  changePage(newPage: number): void {
    if (
      newPage > 0 &&
      newPage <= (this.pagedObject.pagedInfo?.totalPages || 1)
    ) {
      this.currentPage = newPage;
      this.loadEvents();
    }
  }

  updateUrl(): void {
    this.router.navigate([], {
      queryParams: {
        search: this.searchTerm || null,
        page: this.currentPage > 1 ? this.currentPage : null,
        sort: this.sortBy !== 'date' ? this.sortBy : null,
        order: this.sortOrder !== 'desc' ? this.sortOrder : null,
      },
      queryParamsHandling: 'merge',
    });
  }
}

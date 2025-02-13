import { TestBed } from '@angular/core/testing';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { AppComponent } from './app.component';
import { of } from 'rxjs';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterModule, AppComponent], // ✅ Use imports for standalone components
      providers: [
        {
          provide: ActivatedRoute,
          useValue: { params: of({}) }, // ✅ Mock ActivatedRoute
        },
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should have ngOnInit defined', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.ngOnInit).toBeDefined();
  });

  it('should call ngOnInit without errors', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    spyOn(app, 'ngOnInit').and.callThrough();
    app.ngOnInit();
    expect(app.ngOnInit).toHaveBeenCalled();
  });
});

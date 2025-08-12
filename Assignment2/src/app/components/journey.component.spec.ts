import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { JourneyComponent } from './journey.component';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { DataService, Journey } from '../services/data.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('JourneyComponent', () => {
  let component: JourneyComponent;
  let fixture: ComponentFixture<JourneyComponent>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockDataService: jasmine.SpyObj<DataService>;

  beforeEach(async () => {
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockDataService = jasmine.createSpyObj('DataService', [
      'getJourneys',
      'addJourney',
      'updateJourney',
      'deleteJourney'
    ]);

    await TestBed.configureTestingModule({
      imports: [JourneyComponent, CommonModule, FormsModule],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: DataService, useValue: mockDataService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(JourneyComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch journeys on init', () => {
    const journeys: Journey[] = [
      { id: 1, currentPhase: 'Phase 1', startDate: '2025-08-11', status: 'Active' }
    ];
    mockDataService.getJourneys.and.returnValue(of(journeys));

    fixture.detectChanges(); // triggers ngOnInit

    expect(mockDataService.getJourneys).toHaveBeenCalled();
    expect(component.journeys).toEqual(journeys);
  });

  it('should navigate to home when goHome is called', () => {
    component.goHome();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/']);
  });

  it('should open add modal', () => {
    component.openAddModal();
    expect(component.showModal).toBeTrue();
    expect(component.isEditing).toBeFalse();
    expect(component.currentJourney).toEqual({});
  });

  it('should edit journey', () => {
    const journey: Journey = { id: 1, currentPhase: 'Phase 1', startDate: '2025-08-11', status: 'Active' };
    component.editJourney(journey);
    expect(component.showModal).toBeTrue();
    expect(component.isEditing).toBeTrue();
    expect(component.currentJourney).toEqual(journey);
  });

  it('should close modal and reset currentJourney', () => {
    component.showModal = true;
    component.currentJourney = { currentPhase: 'Test' };
    component.closeModal();
    expect(component.showModal).toBeFalse();
    expect(component.currentJourney).toEqual({});
  });

  it('should save new journey when not editing', () => {
    component.isEditing = false;
    component.currentJourney = { currentPhase: 'Phase 1', startDate: '2025-08-11', status: 'Active' };
    component.saveJourney();
    expect(mockDataService.addJourney).toHaveBeenCalled();
  });

  it('should update journey when editing', () => {
    component.isEditing = true;
    component.currentJourney = { id: 1, currentPhase: 'Phase 1', startDate: '2025-08-11', status: 'Active' };
    component.saveJourney();
    expect(mockDataService.updateJourney).toHaveBeenCalled();
  });

  it('should delete journey when confirmed', fakeAsync(() => {
    spyOn(window, 'confirm').and.returnValue(true);
    component.deleteJourney(1);
    tick();
    expect(mockDataService.deleteJourney).toHaveBeenCalledWith(1);
  }));

  it('should not delete journey when cancelled', fakeAsync(() => {
    spyOn(window, 'confirm').and.returnValue(false);
    component.deleteJourney(1);
    tick();
    expect(mockDataService.deleteJourney).not.toHaveBeenCalled();
  }));

  describe('getStatusClass', () => {
    it('should return correct class for active', () => {
      expect(component.getStatusClass('Active')).toBe('status-active');
    });
    it('should return correct class for in progress', () => {
      expect(component.getStatusClass('In Progress')).toBe('status-in-progress');
    });
    it('should return correct class for completed', () => {
      expect(component.getStatusClass('Completed')).toBe('status-completed');
    });
    it('should return correct class for on hold', () => {
      expect(component.getStatusClass('On Hold')).toBe('status-on-hold');
    });
    it('should return default class for unknown status', () => {
      expect(component.getStatusClass('Unknown')).toBe('status-active');
    });
  });
});
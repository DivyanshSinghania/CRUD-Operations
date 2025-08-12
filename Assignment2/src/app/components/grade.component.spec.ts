import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GradeComponent } from './grade.component';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { DataService, Grade } from '../services/data.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('GradeComponent', () => {
  let component: GradeComponent;
  let fixture: ComponentFixture<GradeComponent>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockDataService: jasmine.SpyObj<DataService>;

  const mockGrades: Grade[] = [
    { id: 1, gradeLevel: 'Junior', gradeCode: 'JR01', gradeEffectiveDate: '2025-01-01' },
    { id: 2, gradeLevel: 'Senior', gradeCode: 'SR01', gradeEffectiveDate: '2025-02-01' }
  ];

  beforeEach(async () => {
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);
    mockDataService = jasmine.createSpyObj('DataService', [
      'getGrades',
      'addGrade',
      'updateGrade',
      'deleteGrade'
    ]);

    mockDataService.getGrades.and.returnValue(of(mockGrades));

    await TestBed.configureTestingModule({
      imports: [GradeComponent, CommonModule, FormsModule],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: DataService, useValue: mockDataService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(GradeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load grades on init', () => {
    expect(mockDataService.getGrades).toHaveBeenCalled();
    expect(component.grades.length).toBe(2);
    expect(component.grades[0].gradeLevel).toBe('Junior');
  });

  it('should navigate home when goHome is called', () => {
    component.goHome();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/']);
  });

  it('should open add modal', () => {
    component.openAddModal();
    expect(component.showModal).toBeTrue();
    expect(component.isEditing).toBeFalse();
    expect(component.currentGrade).toEqual({});
  });

  it('should open edit modal with grade data', () => {
    component.editGrade(mockGrades[0]);
    expect(component.showModal).toBeTrue();
    expect(component.isEditing).toBeTrue();
    expect(component.currentGrade.gradeLevel).toBe('Junior');
  });

  it('should close modal and reset currentGrade', () => {
    component.currentGrade = { gradeLevel: 'Test' };
    component.showModal = true;
    component.closeModal();
    expect(component.showModal).toBeFalse();
    expect(component.currentGrade).toEqual({});
  });

  it('should call addGrade when saving new grade', () => {
    component.isEditing = false;
    component.currentGrade = { gradeLevel: 'Mid', gradeCode: 'MD01', gradeEffectiveDate: '2025-03-01' };
    component.saveGrade();
    expect(mockDataService.addGrade).toHaveBeenCalledWith(component.currentGrade as Omit<Grade, 'id'>);
  });

  it('should call updateGrade when saving edited grade', () => {
    component.isEditing = true;
    component.currentGrade = { id: 3, gradeLevel: 'Lead', gradeCode: 'LD01', gradeEffectiveDate: '2025-04-01' };
    component.saveGrade();
    expect(mockDataService.updateGrade).toHaveBeenCalledWith(component.currentGrade as Grade);
  });

  it('should delete grade when confirmed', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    component.deleteGrade(1);
    expect(mockDataService.deleteGrade).toHaveBeenCalledWith(1);
  });

  it('should not delete grade when cancelled', () => {
    spyOn(window, 'confirm').and.returnValue(false);
    component.deleteGrade(1);
    expect(mockDataService.deleteGrade).not.toHaveBeenCalled();
  });
});
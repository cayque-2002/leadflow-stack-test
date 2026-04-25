import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Lead, LeadCreate, LeadUpdate } from '../../core/models/lead.model';
import { LeadStatus } from '../../core/models/lead-status.enum';
import { LeadService } from '../../core/services/lead.service';

import { TaskItem, TaskCreate, TaskUpdate } from '../../core/models/task.model';
import { TaskStatus } from '../../core/models/task-status.enum';
import { TaskService } from '../../core/services/task.service';

@Component({
  selector: 'app-leads',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './leads.component.html',
  styleUrl: './leads.component.scss'
})
export class LeadsComponent implements OnInit {
  leads: Lead[] = [];

  search = '';
  statusFilter: number | null = null;

  selectedLead: Lead | null = null;

  form = {
    name: '',
    email: '',
    status: LeadStatus.New
  };

  loading = false;
  errorMessage = '';
  successMessage = '';

  LeadStatus = LeadStatus;

  selectedLeadTasks: TaskItem[] = [];

  taskForm = {
    title: '',
    dueDate: '',
    status: TaskStatus.Pending
  };

  selectedTask: TaskItem | null = null;

  showTasks = false;

  TaskStatus = TaskStatus;


  constructor(private leadService: LeadService, private taskService: TaskService) {}

  ngOnInit(): void {
    this.loadLeads();
  }

  loadLeads(): void {
    this.loading = true;

    this.leadService.getAll(this.search, this.statusFilter).subscribe({
      next: (data) => {
        this.leads = data;
        this.loading = false;
      },
      error: () => {
        this.showError('Erro ao carregar leads.');
        this.loading = false;
      }
    }); 
  }

  loadTasks(lead: Lead): void {
    this.selectedLead = lead;
    this.showTasks = true;

    this.taskService.getByLead(lead.id).subscribe({
      next: (tasks) => {
        this.selectedLeadTasks = tasks;
      },
      error: () => {
        this.showError('Erro ao carregar tarefas.');
      }
    });
  }

  save(): void {

    if (this.selectedLead) {
      const payload: LeadUpdate = {
        name: this.form.name,
        email: this.form.email,
        status: this.form.status as LeadStatus
      };

      this.leadService.update(this.selectedLead.id, payload).subscribe({
        next: () => {
          this.showSuccess('Lead atualizado com sucesso.');
          this.resetForm();
          this.loadLeads();
          this.closeLeadModal();
        },
        error: (err) => {
          this.showError(err?.error?.message ?? 'Erro ao atualizar lead.');
        }
      });

      return;
    }

    const payload: LeadCreate = {
      name: this.form.name,
      email: this.form.email,
      status: this.form.status as LeadStatus
    };

    this.leadService.create(payload).subscribe({
      next: () => {
        this.showSuccess('Lead criado com sucesso.');
        this.resetForm();
        this.loadLeads();
      },
      error: (err) => {
        this.showError(err?.error?.message ?? 'Erro ao criar lead.');
      }
    });
  }

  saveTask(): void {
    if (!this.selectedLead) return;

    if (this.selectedTask) {
      const payload: TaskUpdate = {
        title: this.taskForm.title,
        dueDate: this.taskForm.dueDate || null,
        status: Number(this.taskForm.status)
      };

      this.taskService
        .update(this.selectedLead.id, this.selectedTask.id, payload)
        .subscribe({
          next: () => {
            this.showSuccess('Tarefa atualizada.');
            this.resetTaskForm();
            this.loadTasks(this.selectedLead!);
          },
          error: () => {
            this.showError('Erro ao atualizar tarefa.');
          }
        });

        this.loadLeads();

      return;
    }

    const payload: TaskCreate = {
      title: this.taskForm.title,
      dueDate: this.taskForm.dueDate || null,
      status: Number(this.taskForm.status)
    };

    this.taskService.create(this.selectedLead.id, payload).subscribe({
      next: () => {
        this.showSuccess('Tarefa criada.');
        this.resetTaskForm();
        this.loadTasks(this.selectedLead!);
        this.loadLeads();
      },
      error: () => {
        this.showError('Erro ao criar tarefa.');
      }
    });
  }

  edit(lead: Lead): void {
    this.selectedLead = lead;

    this.form = {
      name: lead.name,
      email: lead.email,
      status: lead.status
    };

    this.showLeadModal = true;
  }

  editTask(task: TaskItem): void {
    this.selectedTask = task;

    this.taskForm = {
      title: task.title,
      dueDate: task.dueDate ?? '',
      status: task.status
    };
  }

  delete(id: number): void {
    if (!confirm('Deseja realmente excluir este lead?')) {
      return;
    }

    this.leadService.delete(id).subscribe({
      next: () => {
        this.showSuccess('Lead excluído com sucesso.');
        this.loadLeads();
      },
      error: () => {
        this.showError('Erro ao excluir lead.');
      }
    });
  }

  deleteTask(task: TaskItem): void {
    if (!this.selectedLead) return;

    if (!confirm('Excluir tarefa?')) return;

    this.taskService
      .delete(this.selectedLead.id, task.id)
      .subscribe({
        next: () => {
          this.showSuccess('Tarefa excluída.');
          this.loadTasks(this.selectedLead!);
          this.loadLeads(); 
        },
        error: () => {
          this.showError('Erro ao excluir tarefa.');
        }
      });
  }

  resetForm(): void {
    this.selectedLead = null;

    this.form = {
      name: '',
      email: '',
      status: LeadStatus.New
    };
  }

  resetTaskForm(): void {
    this.selectedTask = null;

    this.taskForm = {
      title: '',
      dueDate: '',
      status: TaskStatus.Pending
    };
  }

  clearFilters(): void {
    this.search = '';
    this.statusFilter = null;
    this.loadLeads();
  }

  getLeadsByStatus(status: LeadStatus): Lead[] {
    return this.leads.filter(lead => lead.status === status);
  }

  getStatusLabel(status: LeadStatus): string {
    const labels: Record<LeadStatus, string> = {
      [LeadStatus.New]: 'Novo',
      [LeadStatus.Contacted]: 'Contatado',
      [LeadStatus.Qualified]: 'Qualificado',
      [LeadStatus.Lost]: 'Perdido'
    };

    return labels[status];
  }

  getStatusClass(status: LeadStatus): string {
    const classes: Record<LeadStatus, string> = {
      [LeadStatus.New]: 'status-new',
      [LeadStatus.Contacted]: 'status-contacted',
      [LeadStatus.Qualified]: 'status-qualified',
      [LeadStatus.Lost]: 'status-lost'
    };

    return classes[status];
  }

  showSuccess(message: string): void {
    this.successMessage = message;

    setTimeout(() => {
      this.successMessage = '';
    }, 3000); // 3 segundos
  }

  showError(message: string): void {
    this.errorMessage = message;

    setTimeout(() => {
      this.errorMessage = '';
    }, 4000);
  }

  clearMessages(): void {
    this.successMessage = '';
    this.errorMessage = '';
  }

  showLeadModal = false;

  openLeadModal(): void {
    this.clearMessages();
    this.resetForm();
    this.showLeadModal = true;
  }

  closeLeadModal(): void {
    this.clearMessages();
    this.showLeadModal = false;
    this.resetForm();
  }

  showTaskModal = false;

  openTaskModal(lead: Lead): void {
    this.clearMessages();
    this.selectedLead = lead;
    this.selectedLeadTasks = [];
    this.showTaskModal = true;
    this.resetTaskForm();

    this.taskService.getByLead(lead.id).subscribe({
      next: (tasks) => {
        this.selectedLeadTasks = tasks;
      },
      error: () => {
        this.errorMessage = 'Erro ao carregar tarefas.';
      }
    });
  }

  closeTaskModal(): void {
    this.clearMessages();
    this.showTaskModal = false;
    this.resetTaskForm();
  }

}
import { LeadStatus } from './lead-status.enum';

export interface Lead {
  id: number;
  name: string;
  email: string;
  status: LeadStatus;
  createdAt: string;
  updatedAt: string;
  tasksCount?: number;
}

export interface LeadCreate {
  name: string;
  email: string;
  status?: LeadStatus | null;
}

export interface LeadUpdate {
  name: string;
  email: string;
  status: LeadStatus;
}
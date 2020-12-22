import { HSMMealType } from "./Meal";
import RawHSMService from "./Service";
import { Type } from "class-transformer";
import HSMService from "./Service";

export class HSMCustomerTag {
  id!: number;
  name!: string;
}

export interface RawHSMServiceAssignments {
  [HSMMealType.Breakfast]: RawHSMService;
  [HSMMealType.Lunch]: RawHSMService;
  [HSMMealType.Dinner]: RawHSMService;
}

export class HSMServiceAssignments {
  @Type(() => HSMService)
  [HSMMealType.Breakfast]: HSMService;
  @Type(() => HSMService)
  [HSMMealType.Lunch]: HSMService;
  @Type(() => HSMService)
  [HSMMealType.Dinner]: HSMService;
}

export interface RawHSMCustomer {
  id: number;
  name: string;
  hidden: boolean;
  discharged: boolean;
  admission_date: string;
  discharge_date?: string;
  note?: string;
  tags: string[];
  serviceAssignments?: RawHSMServiceAssignments;
  created_at: string;
  last_updated_at: string;
}

export default class HSMCustomer {
  id!: number;
  name!: string;
  hidden!: boolean;
  discharged!: boolean;
  @Type(() => Date)
  admission_date!: Date;
  @Type(() => Date)
  discharge_date?: Date;
  note?: string;
  @Type(() => String)
  tags!: string[];
  @Type(() => HSMServiceAssignments)
  serviceAssignments?: HSMServiceAssignments;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface HSMCustomerInputModel {
  name: string;
  hidden: boolean;
  discharged: boolean;
  admission_date: Date;
  discharge_date?: Date;
  tags: string[];
  note?: string;
}

export interface HSMCustomerOptionsInputModel {
  visible?: boolean;
  discharged?: boolean;
  discharged_date?: Date;
}

export interface HSMCustomerServiceAssignmentInput {
  [HSMMealType.Breakfast]?: number;
  [HSMMealType.Lunch]?: number;
  [HSMMealType.Dinner]?: number;
}

export interface HSMCustomerServiceInputModel {
  services: HSMCustomerServiceAssignmentInput;
}

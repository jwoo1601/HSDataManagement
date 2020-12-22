import TimeSpan from "../TimeSpan";
import { Transform, Type } from "class-transformer";

export interface RawHSMServiceGroup {
  id: number;
  name: string;
  note?: string;
  created_at: string;
  last_updated_at: string;
}

export class HSMServiceGroup {
  id!: number;
  name!: string;
  note?: string;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface RawHSMService {
  id: number;
  name: string;
  duration?: string;
  note?: string;
  group?: RawHSMServiceGroup;
  created_at: string;
  last_updated_at: string;
}

export default class HSMService {
  id!: number;
  name!: string;
  @Type(() => TimeSpan)
  @Transform((value) => new TimeSpan(value), { toClassOnly: true })
  duration!: TimeSpan;
  note?: string;
  @Type(() => HSMServiceGroup)
  group?: HSMServiceGroup;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface HSMServiceInputModel {
  name: string;
  note?: string;
  duration?: string;
  group?: number;
}

export interface HSMServiceGroupInputModel {
  name: string;
  note?: string;
}

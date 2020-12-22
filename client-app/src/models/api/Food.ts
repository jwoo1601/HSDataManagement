import { Type } from "class-transformer";

export interface RawHSMFoodCategory {
  id: number;
  name: string;
  note?: string;
  created_at: string;
  last_updated_at: string;
}

export class HSMFoodCategory {
  id!: number;
  name!: string;
  note?: string;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface RawHSMFoodIngredientCategory {
  id: number;
  name: string;
  note?: string;
  created_at: string;
  last_updated_at: string;
}

export class HSMFoodIngredientCategory {
  id!: number;
  name!: string;
  note?: string;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface RawHSMFoodIngredient {
  id: number;
  name: string;
  origin?: string;
  category?: RawHSMFoodIngredientCategory;
  created_at: string;
  last_updated_at: string;
}

export class HSMFoodIngredient {
  id!: number;
  name!: string;
  origin?: string;
  @Type(() => HSMFoodIngredientCategory)
  category?: HSMFoodIngredientCategory;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface RawHSMFood {
  id: number;
  name: string;
  note: string;
  category?: RawHSMFoodCategory;
  ingredients: RawHSMFoodIngredient[];
  created_at: string;
  last_updated_at: string;
}

export default class HSMFood {
  id!: number;
  name!: string;
  note?: string;
  @Type(() => HSMFoodCategory)
  category?: HSMFoodCategory;
  @Type(() => HSMFoodIngredient)
  ingredients!: HSMFoodIngredient[];
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface HSMFoodInputModel {
  name: string;
  note?: string;
  category: number;
  ingredients: number[];
}

export interface HSMFoodCategoryInputModel {
  name: string;
  note?: string;
}

export interface HSMFoodIngredientInputModel {
  name: string;
  origin?: string;
  category: number;
}

export interface HSMFoodIngredientCategoryInputModel {
  name: string;
  note?: string;
}

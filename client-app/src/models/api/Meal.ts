import HSMFood from "./Food";

export interface HSMMealPackage {
  id: number;
  name: string;
  note: string;
  calories: number;
  proteinAmount: number;
  meals: HSMMeal[];
}

export interface HSMMeal {
  id: number;
  name: string;
  note: string;
  type: HSMMealType;
  foods: HSMFood[];
}

export enum HSMMealType {
  Breakfast = "breakfast",
  Lunch = "lunch",
  Dinner = "dinner",
}

export interface HSMOperationLog {
  id: Int16Array;
  date: Date;
  breakfast: HSMMealOperationLog;
  lunch: HSMMealOperationLog;
  dinner: HSMMealOperationLog;
}

export interface HSMMealOperationLog {
  numCustomersServed: number;
  numEmployeesServed: number;
}

export * from './northwind.service';
import { NorthwindService } from './northwind.service';
export * from './values.service';
import { ValuesService } from './values.service';
export const APIS = [NorthwindService, ValuesService];

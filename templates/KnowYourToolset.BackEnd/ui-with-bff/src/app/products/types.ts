export interface Product {
    id?: number;
    name: string;
    price: number;
    department: string;
    manufacturer: string;
    distributor: string;
    isActive: boolean;
}

export interface PagedProducts {
  items: Product[];
  pageNumber: number;
  pageSize: number
  totalPages: number;
  totalCount: number;
  first: string;
  last: string;
  prev: string;
  next: string;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}


export interface ProductById extends Product {
  categories: string[];
  barcode: string;
  createdDate: string;
  updatedDate: string;
  createdBy: string;
}
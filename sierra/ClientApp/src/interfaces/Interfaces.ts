interface ICustomer {
    id?: number;
    name: string;
}

interface IProduct {
    id?: number;
    name: string;
    price: number;
}

interface IOrder {
    id?: number;
    customer: ICustomer;
    product: IProduct;
    quantity: number;
    price: number;
    total: number;
}

export type {
    ICustomer,
    IProduct,
    IOrder
}
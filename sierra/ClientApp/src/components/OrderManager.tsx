import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { Button, Form, Row, Spinner, Table } from "reactstrap"
import { ICustomer, IOrder, IProduct } from "../interfaces/Interfaces";
import useAjaxService from "../services/AjaxService";
import CustomInput from "./common/CustomInput";

const OrderManager = () => {
    const { ajaxGet, ajaxPost } = useAjaxService();
    const [items, setItems] = useState<IOrder[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const { control, handleSubmit, reset } = useForm({ defaultValues: { productId: null, customerId: null, quantity: 1 } });
    const [customers, setCustomers] = useState<ICustomer[]>([]);
    const [products, setProducts] = useState<IProduct[]>([]);

    useEffect(() => {
        reloadItems();
    }, []);

    const reloadItems = async () => {
        try {
            setLoading(true);
            const _items = await ajaxGet("api/order");
            setItems(_items);
            const _products = await ajaxGet("api/product");
            setProducts(_products);
            const _customers = await ajaxGet("api/customer");
            setCustomers(_customers);
        } catch (error) {
            console.error("Order GET. Unexpected error", error);
            alert("An unexpected error occurred, please review console log");
        } finally {
            setLoading(false);
        }
    }

    const submit = async (data: any) => {
        try {
            setLoading(true);
            await ajaxPost("api/order", true, { ...data, quantity: parseInt(data.quantity) });
            reset();
            reloadItems();
            alert("Order added successfully");
        } catch (error) {
            alert(error);
        } finally {
            setLoading(false);
        }
    }

    return (<>
        <Form onSubmit={handleSubmit(submit)}>
            <Row xs={2} md={3}>
                <CustomInput
                    control={control}
                    type="select"
                    name="customerId"
                    label="Customer"
                    required={true}
                    addEmptyOption={true}
                    options={customers.map(c => (<option key={`customer-${c.id}`} value={c.id}>{c.name}</option>))}
                />
                <CustomInput
                    type="select"
                    name="productId"
                    label="Product"
                    control={control}
                    required={true}
                    addEmptyOption={true}
                    options={products.map(p => (<option key={`product-${p.id}`} value={p.id}>{`$ ${p.price} - ${p.name}`}</option>))}
                />
                <CustomInput type="number" name="quantity" label="Quantity" control={control} required={true} min={1} max={1000} isInt={true} />
            </Row>
            <br/>
            {
                loading
                    ? (<Spinner> </Spinner>)
                    : (<Button type="submit" color="primary">Add</Button>)
            }
        </Form>
        <Table>
            <thead>
                <tr>
                    <th>
                        Id
                    </th>
                    <th>
                        Customer
                    </th>
                    <th>
                        Product
                    </th>
                    <th>
                        Quantity
                    </th>
                    <th>
                        Price
                    </th>
                    <th>
                        Total
                    </th>
                </tr>
            </thead>
            <tbody>
                {
                    items.map(p => (
                        <tr key={p.id}>
                            <td>
                                {p.id}
                            </td>
                            <td>
                                {p.customer.name}
                            </td>
                            <td>
                                {p.product.name}
                            </td>
                            <td>
                                {p.quantity}
                            </td>
                            <td>
                                {p.price}
                            </td>
                            <td>
                                {p.total}
                            </td>
                        </tr>
                    ))
                }
            </tbody>
        </Table>
    </>);
}

export default OrderManager;
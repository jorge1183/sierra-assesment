import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { Button, ButtonGroup, Form, Row, Spinner, Table } from "reactstrap"
import { ICustomer } from "../interfaces/Interfaces";
import useAjaxService from "../services/AjaxService";
import CustomInput from "./common/CustomInput";

const CustomerManager = () => {
    const { ajaxGet, ajaxPost, ajaxPut } = useAjaxService();
    const [items, setItems] = useState<ICustomer[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const { control, handleSubmit, reset, setFocus, watch } = useForm({ defaultValues: {id: 0, name: "", price: 0 } });
    const watchId = watch("id");

    useEffect(() => {
        reloadItems();
    }, []);

    const reloadItems = async () => {
        try {
            setLoading(true);
            const _items = await ajaxGet("api/customer");
            setItems(_items);
        } catch (error) {
            console.error("Customer GET. Unexpected error", error);
            alert("An unexpected error occurred, please review console log");
        } finally {
            setLoading(false);
        }
    }

    const submit = async (data: ICustomer) => {
        try {
            setLoading(true);
            if (data.id) {
                await ajaxPut("api/customer", data.id, data, true);
            } else {
                await ajaxPost("api/customer", data, true);
            }
            reset();
            reloadItems();
            alert(`Customer ${data.id ? "modified" : "added"} successfully`);
        } catch (error) {
            alert(error);
        } finally {
            setLoading(false);
        }
    }

    const editItem = (data: ICustomer) => {
        setFocus("name");
        reset(data, { keepDefaultValues: true });
    }

    return (<>
        <Form onSubmit={handleSubmit(submit)}>
            <Row xs={2} md={3}>
                <CustomInput type="text" name="name" label="Customer Name" control={control} required={true} maxLength={20} />
            </Row>
            <br/>
            {
                loading
                    ? (<Spinner> </Spinner>)
                    : (
                        <ButtonGroup>
                            <Button type="submit" color="primary">{watchId ? "Save" : "Add"}</Button>
                            {
                                watchId > 0 && <Button type="button" onClick={() => reset()}>Cancel</Button>
                            }
                        </ButtonGroup>
                    )
            }
        </Form>
        <Table>
            <thead>
                <tr>
                    <th>
                        Id
                    </th>
                    <th>
                        Name
                    </th>
                    <th></th>
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
                                {p.name}
                            </td>
                            <td>
                                <Button type="button" size="sm" onClick={() => editItem(p)}>Modify</Button>
                            </td>
                        </tr>
                    ))
                }
            </tbody>
        </Table>
    </>);
}

export default CustomerManager;
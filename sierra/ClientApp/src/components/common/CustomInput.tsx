import { useController } from 'react-hook-form';
import { FormFeedback, FormGroup, Input, Label } from 'reactstrap';
import { InputType } from 'reactstrap/es/Input';

interface ICustomInputProps {
    control?: any;
    label: string;
    name: string;
    defaultValue?: any;
    type?: InputType;
    required?: true;
    maxLength?: number;
    min?: number;
    max?: number;
    isInt?: boolean;
    options?: any;
    addEmptyOption?: boolean;
    [key: string]: any;
}
const CustomInput = (
    {
        control, label, name, defaultValue, type, required, maxLength, max, min, isInt, options, addEmptyOption, ...remainingProps
    }: ICustomInputProps) => {
    const maxLengthValidation = (value: string) => {
        const result = (maxLength != null && value != null && value.length > maxLength && `Max length (${maxLength}) exceeded`) || true;
        return result;
    }
    const maxValueValidation = (value: number) => {
        const result = (max != null && value != null && value > max && `Max value (${max}) exceeded`) || true;
        return result;
    }
    const minValueValidation = (value: number) => {
        const result = (min != null && value != null && value < min && `Min value (${min}) not reached`) || true;
        return result;
    }
    const isIntValidation = (value: string) => {
        const result = (value == null || Number.isInteger(Number.parseFloat(value)) || `Value should be an integer`);
        return result;
    }

    let validate = {};
    validate = maxLength ? { ...validate, maxLengthValidation } : validate;
    validate = max ? { ...validate, maxValueValidation } : validate;
    validate = min ? { ...validate, minValueValidation } : validate;
    validate = isInt ? { ...validate, isIntValidation } : validate;

    const { field, fieldState } = useController({
        control: control,
        name: name,
        rules: { required, validate }
    })

    const inputProps = {
        type: type ?? "text",
        id: field.name,
        name: field.name,
        onBlur: field.onBlur,
        onChange: field.onChange,
        value: field.value ?? defaultValue ?? "",
        innerRef: field.ref,
        invalid: fieldState.invalid,
        ...remainingProps
    };
    return (
        <FormGroup>
            <Label for={name}>{label}</Label>
            {
                type === "select"
                    ? (
                        <Input {...inputProps}>
                            {addEmptyOption ? [(<option key={`${name}-select-default`} value="" >-- Select --</option>), ...options] : options}
                        </Input>
                    )
                    : (<Input {...inputProps} />)
            }
            {
                fieldState.error && (
                    <FormFeedback >
                        {
                            fieldState.error.message ||
                            (fieldState.error.type === "required" && "Field is required") ||
                            fieldState.error.type
                        }
                    </FormFeedback>
                )
            }
        </FormGroup>);
};

export default CustomInput;
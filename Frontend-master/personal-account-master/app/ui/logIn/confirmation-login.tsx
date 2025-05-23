import { Input } from "antd"

interface Props {
  value: string,
  setValue: React.Dispatch<React.SetStateAction<string>>,
}

export const ConfirmationInput = ({value, setValue}: Props) => {

  return (
    <Input.OTP
      type="text"
      size="large"
      className=""
      onChange={(value) => setValue(value)}
      value={value}
    ></Input.OTP>
  )
}

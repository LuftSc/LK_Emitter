import { create } from "zustand"

type FormStore = {
  inputText: string
  setInputText: (value: string) => void
}

const useFormStore = create<FormStore>()((set) => ({
  inputText: "",
  setInputText: (value) => set({ inputText: value }),
}))

export default useFormStore

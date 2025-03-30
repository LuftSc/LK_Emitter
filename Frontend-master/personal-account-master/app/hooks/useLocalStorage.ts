'use client';

export function useSimpleStorage(key: string) {
  const setItem = (value: any) => {
    localStorage.setItem(key, JSON.stringify(value));
  };

  const getItem = () => {
    const data = localStorage.getItem(key);
    return data ? JSON.parse(data) : null;
  };

  return { setItem, getItem };
}
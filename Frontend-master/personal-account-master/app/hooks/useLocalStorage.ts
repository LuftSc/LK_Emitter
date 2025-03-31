/*'use client';

export function useSimpleStorage(key: string) {
  const setItem = (value: any) => {
    localStorage.setItem(key, JSON.stringify(value));
  };

  const getItem = () => {
    const data = localStorage.getItem(key);
    return data ? JSON.parse(data) : null;
  };

  const removeItem = () => {
    try {
      localStorage.removeItem(key);
    } catch (error) {
      console.error('Error removing from localStorage', error);
    }
  };

  const clearAll = () => {
    try {
      localStorage.clear();
    } catch (error) {
      //console.error('Error clearing localStorage', error);
    }
  };

  return { setItem, getItem, removeItem, clearAll };
} */
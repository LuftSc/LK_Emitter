if (typeof window !== 'undefined') {
    const originalWarn = console.warn;
    console.warn = (...args: unknown[]) => {
      if (typeof args[0] === 'string' && args[0].includes('antd: compatible')) {
        return;
      }
      originalWarn.apply(console, args);
    };
  }
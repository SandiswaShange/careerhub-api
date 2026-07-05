import "@testing-library/jest-dom/vitest";

const localStorageMock = {
  getItem: () => null,
  setItem: () => {},
  removeItem: () => {},
  clear: () => {},
};

Object.defineProperty(globalThis, "localStorage", {
  value: localStorageMock,
  writable: true,
});
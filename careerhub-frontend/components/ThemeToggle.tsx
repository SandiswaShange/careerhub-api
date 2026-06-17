"use client";

import { useEffect } from "react";

export function ThemeToggle() {
  // INITIAL LOAD
  useEffect(() => {
    const stored = localStorage.getItem("theme");

    if (stored === "dark") {
      document.documentElement.classList.add("dark");
      return;
    }

    if (stored === "light") {
      document.documentElement.classList.remove("dark");
      return;
    }

    const prefersDark = window.matchMedia(
      "(prefers-color-scheme: dark)"
    ).matches;

    if (prefersDark) {
      document.documentElement.classList.add("dark");
    }
  }, []);

  function toggleTheme() {
    const root = document.documentElement;
    const isDark = root.classList.contains("dark");

    if (isDark) {
      root.classList.remove("dark");
      localStorage.setItem("theme", "light");
    } else {
      root.classList.add("dark");
      localStorage.setItem("theme", "dark");
    }
  }

  return (
    <button
      onClick={toggleTheme}
      aria-label="Toggle theme"
      className="px-3 py-2 border rounded bg-white dark:bg-slate-900 dark:text-white"
    >
      Toggle Theme
    </button>
  );
}
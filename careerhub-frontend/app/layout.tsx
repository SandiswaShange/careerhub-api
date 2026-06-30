import type { Metadata } from "next";
import Link from "next/link";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import Providers from "./providers";
import { ThemeToggle } from "@/components/ThemeToggle";
import { Toaster } from "sonner";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "CareerHub",
  description: "CareerHub Job Portal",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html
      lang="en"
      className={`${geistSans.variable} ${geistMono.variable} h-full antialiased`}
    >
        <body className="min-h-full flex flex-col">
    <Providers>
      <header className="border-b border-slate-200 dark:border-slate-800">
        <div className="flex items-center justify-between p-4">
          <Link
            href="/"
            className="text-xl font-bold text-slate-900 dark:text-slate-100"
          >
            CareerHub
          </Link>

          <div className="flex items-center gap-6">
            <nav className="flex gap-4">
              <Link
                href="/jobs"
                className="text-slate-700 hover:underline dark:text-slate-300"
              >
                Jobs
              </Link>

              <Link
                href="/dashboard/listings"
                className="text-slate-700 hover:underline dark:text-slate-300"
              >
                Dashboard
              </Link>
            </nav>

            <ThemeToggle />
          </div>
        </div>
      </header>

      <main className="flex-1">
        {children}
      </main>
    </Providers>

    <Toaster
      position="top-right"
      richColors
      closeButton
    />
  </body>
    </html>
  );
}
import type {Metadata} from "next";
import "./globals.css";
import NavBar from "@/app/nav/NavBar";

export const metadata: Metadata = {
  title: "AuctionNext",
  description: "Generated by create next app",
};

export default function RootLayout({children,}: Readonly<{
  children: any
}>) {
  return (
      <html lang="en">
      <body>
      <NavBar/>
      <main className="container mx-auto px-5 pt-10">
        {children}
      </main>
      </body>
      </html>
  );
}

import * as React from 'react';
import AuctionCard from "@/app/auctions/AuctionCard";
import {Auction, PagedResult} from "@/types";

/**
 * Asynchronously fetches auction data from the server.
 *
 * This function sends a GET request to a predefined URL, expecting to fetch a paged list of auction items.
 * It handles the network response, ensuring that any non-OK response throws an error.
 *
 * @return {Promise<PagedResult<Auction>>} A promise that resolves to a paged result containing auction items.
 * @throws {Error} When the fetch operation fails or the response status is not OK.
 */
async function getData(): Promise<PagedResult<Auction>> {
  // Sends a fetch request to the server to get auction data.
  const res = await fetch('http://localhost:6001/search?pageSize=10');

  // Checks if the response was not OK and throws an error.
  if (!res.ok) throw new Error('Failed to fetch data');

  // Parses the JSON response and returns it as a promise.
  return res.json();
}

/**
 * The Listings component asynchronously fetches auction listings and renders them using AuctionCard components.
 *
 * This component makes use of the `getData` function to fetch data on mount and then renders a grid of AuctionCard
 * components, each representing an auction. The grid layout is achieved using Tailwind CSS classes.
 *
 * @return {Promise<JSX.Element>} A promise that resolves to the JSX.Element representing the listings grid.
 */
export default async function Listings(): Promise<JSX.Element> {
  // Fetches auction data using the getData function.
  const data = await getData();

  // Renders the auction data as a grid of AuctionCard components.
  return (
      <div className="grid grid-cols-4 gap-6">
        {data && data.results.map(auction => (
            <AuctionCard key={auction.id} auction={auction}/>
        ))}
      </div>
  );
}
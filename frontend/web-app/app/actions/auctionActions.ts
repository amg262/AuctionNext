'use server';

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
export async function getData(query: string): Promise<PagedResult<Auction>> {
  // Sends a fetch request to the server to get auction data.
  const res = await fetch(`http://localhost:6001/search${query}`);

  // Checks if the response was not OK and throws an error.
  if (!res.ok) throw new Error('Failed to fetch data');

  // Parses the JSON response and returns it as a promise.
  return res.json();
}
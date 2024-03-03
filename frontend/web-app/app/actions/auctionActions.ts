'use server';

import {Auction, Bid, PagedResult} from "@/types";
import {fetchWrapper} from "@/lib/fetchWrapper";
import {FieldValues} from "react-hook-form";
import {revalidatePath} from "next/cache";

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
  return await fetchWrapper.get(`search${query}`);
}

export async function updateAuctionTest() {
  const data = {
    make: 'Lexus',
    mileage: Math.floor(Math.random() * 100000) + 1
  }
  return await fetchWrapper.put('auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', data);
}

export async function createAuction(data: FieldValues) {
  return await fetchWrapper.post('auctions', data);
}

export async function getDetailedViewData(id: string): Promise<Auction> {
  return await fetchWrapper.get(`auctions/${id}`);
}

export async function updateAuction(data: FieldValues, id: string): Promise<Auction> {
  const res = await fetchWrapper.put(`auctions/${id}`, data);
  revalidatePath(`/auctions/${id}`);
  return res;
}

export async function deleteAuction(id: string): Promise<any> {
  return await fetchWrapper.del(`auctions/${id}`);
}

export async function getBidsForAuction(id: string): Promise<Bid[]> {
  return await fetchWrapper.get(`bids/${id}`);
}
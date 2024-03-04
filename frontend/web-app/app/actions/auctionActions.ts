'use server'

import {Auction, Bid, PagedResult} from "@/types";
import {fetchWrapper} from "@/app/lib/fetchWrapper";
import {FieldValues} from "react-hook-form";
import {revalidatePath} from "next/cache";

/**
 * Retrieves a paginated result of auctions based on a query string.
 * @param query The search query string used to filter auctions.
 * @returns A Promise that resolves to a `PagedResult<Auction>` containing the search results.
 */
export async function getData(query: string): Promise<PagedResult<Auction>> {
  return await fetchWrapper.get(`search/${query}`)
}

/**
 * Updates an auction with random mileage. This function is primarily for test purposes.
 * @returns A Promise that resolves to the response of the update operation.
 */
export async function updateAuctionTest() {
  const data = {
    mileage: Math.floor(Math.random() * 100000) + 1
  }

  return await fetchWrapper.put('auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', data);
}

/**
 * Creates a new auction with the provided data.
 * @param data The `FieldValues` containing auction data to be created.
 * @returns A Promise that resolves to the response of the create operation.
 */
export async function createAuction(data: FieldValues) {
  return await fetchWrapper.post('auctions', data);
}

/**
 * Fetches detailed data for a specific auction by its ID.
 * @param id The unique identifier of the auction.
 * @returns A Promise that resolves to an `Auction` object containing detailed information about the auction.
 */
export async function getDetailedViewData(id: string): Promise<Auction> {
  return await fetchWrapper.get(`auctions/${id}`);
}

/**
 * Updates an existing auction with the provided data.
 * @param data The `FieldValues` containing updated data for the auction.
 * @param id The unique identifier of the auction to be updated.
 * @returns A Promise that resolves to the response of the update operation.
 */
export async function updateAuction(data: FieldValues, id: string) {
  const res = await fetchWrapper.put(`auctions/${id}`, data);
  revalidatePath(`/auctions/${id}`);
  return res;
}

/**
 * Deletes an auction by its ID.
 * @param id The unique identifier of the auction to be deleted.
 * @returns A Promise that resolves to the response of the delete operation.
 */
export async function deleteAuction(id: string) {
  return await fetchWrapper.del(`auctions/${id}`);
}

/**
 * Retrieves all bids for a specific auction.
 * @param id The unique identifier of the auction for which bids are being fetched.
 * @returns A Promise that resolves to an array of `Bid` objects associated with the auction.
 */
export async function getBidsForAuction(id: string): Promise<Bid[]> {
  return await fetchWrapper.get(`bids/${id}`);
}

/**
 * Places a bid on an auction.
 * @param auctionId The unique identifier of the auction on which the bid is being placed.
 * @param amount The amount of the bid.
 * @returns A Promise that resolves to the response of the bid placement operation.
 */
export async function placeBidForAuction(auctionId: string, amount: number) {
  return await fetchWrapper.post(`bids?auctionId=${auctionId}&amount=${amount}`, {})
}
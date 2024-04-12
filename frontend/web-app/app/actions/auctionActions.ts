'use server'

import {Auction, Bid, PagedResult} from "@/types";
import {fetchWrapper} from "@/app/lib/fetchWrapper";
import {FieldValues} from "react-hook-form";
import {revalidatePath} from "next/cache";

/**
 * Fetches data based on a given search query.
 * @param query The search query string.
 * @returns A promise that resolves with the paged result of auctions.
 */
export async function getData(query: string): Promise<PagedResult<Auction>> {
  return await fetchWrapper.get(`search/${query}`)
}

export async function updatePost(data: FieldValues, id: string) {
  return await fetchWrapper.put(`post/${id}`, data);
}

export async function createPost(data: FieldValues) {
  return await fetchWrapper.post('post', data);
}

/**
 * Fetches a specific post by its ID.
 * @param id The unique identifier of the post.
 * @returns A promise that resolves with the post data.
 */
export async function getPost(id: string) {
  return await fetchWrapper.get(`post/${id}`);
}

/**
 * Fetches all posts available.
 * @returns A promise that resolves with all posts.
 */
export async function getPosts() {
  return await fetchWrapper.get(`post`);
}

/**
 * Retrieves a coupon by its code.
 * @param code The coupon code to retrieve.
 * @returns A promise that resolves with the coupon data.
 */
export async function getCoupon(code: string) {
  return await fetchWrapper.get(`coupon/code/${code}`);
}

/**
 * Fetches payment information.
 * @returns A promise that resolves with payment details.
 */
export async function getPayment() {
  return await fetchWrapper.get("payment");
}

/**
 * Validates and completes a payment by its ID.
 * @param id The payment ID to validate.
 * @returns A promise that confirms the payment validation.
 */
export async function completePayment(id: string) {
  return await fetchWrapper.post(`payment/validate?paymentId=${id}`, {});
}

/**
 * Retrieves payment information by payment ID.
 * @param id The payment ID.
 * @returns A promise that resolves with the payment details.
 */
export async function getPaymentById(id: string) {
  return await fetchWrapper.get(`payment/${id}`);
}

/**
 * Creates a new payment.
 * @param data The payment data.
 * @returns A promise that resolves with the created payment details.
 */
export async function createPayment(data: any) {
  console.log("createPayment", {data});
  return await fetchWrapper.post("payment/create", data);
}

/**
 * Tests update functionality on a predefined auction by setting random mileage.
 * @returns A promise that resolves with the update confirmation.
 */
export async function updateAuctionTest() {
  const data = {
    mileage: Math.floor(Math.random() * 100000) + 1
  }
  return await fetchWrapper.put('auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', data);
}

/**
 * Creates a new auction with the provided data.
 * @param data The data for the auction.
 * @returns A promise that resolves with the details of the created auction.
 */
export async function createAuction(data: FieldValues) {
  return await fetchWrapper.post('auctions', data);
}

/**
 * Fetches detailed data for a specific auction by ID.
 * @param id The unique identifier for the auction.
 * @returns A promise that resolves with detailed auction data.
 */
export async function getDetailedViewData(id: string): Promise<Auction> {
  return await fetchWrapper.get(`auctions/${id}`);
}

/**
 * Updates an auction with new data.
 * @param data New data to update the auction.
 * @param id The auction's unique identifier.
 * @returns A promise that resolves with the updated auction data.
 */
export async function updateAuction(data: FieldValues, id: string) {
  const res = await fetchWrapper.put(`auctions/${id}`, data);
  revalidatePath(`/auctions/${id}`);
  return res;
}

/**
 * Deletes an auction by its ID.
 * @param id The unique identifier of the auction to delete.
 * @returns A promise that resolves with the result of the deletion.
 */
export async function deleteAuction(id: string) {
  return await fetchWrapper.del(`auctions/${id}`);
}

/**
 * Fetches all bids for a specific auction.
 * @param id The auction's unique identifier.
 * @returns A promise that resolves with an array of bids for the auction.
 */
export async function getBidsForAuction(id: string): Promise<Bid[]> {
  return await fetchWrapper.get(`bids/${id}`);
}

/**
 * Places a bid on an auction.
 * @param auctionId The ID of the auction to bid on.
 * @param amount The amount of the bid.
 * @returns A promise that resolves with the bid confirmation.
 */
export async function placeBidForAuction(auctionId: string, amount: number) {
  return await fetchWrapper.post(`bids?auctionId=${auctionId}&amount=${amount}`, {})
}
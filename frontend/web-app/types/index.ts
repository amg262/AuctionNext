export type PagedResult<T> = {
  results: T[]
  pageCount: number
  totalCount: number
}

export type Auction = {
  reservePrice: number
  seller: string
  winner?: string
  soldAmount: number
  currentHighBid: number
  createdAt: string
  updatedAt: string
  auctionEnd: string
  status: string
  make: string
  model: string
  year: number
  color: string
  mileage: number
  imageUrl: string
  id: string
}

export type Bid = {
  id: string
  auctionId: string
  bidder: string
  bidTime: string
  amount: number
  bidStatus: string
}

export type AuctionFinished = {
  itemSold: boolean
  auctionId: string
  winner?: string
  seller: string
  amount?: number
}

export type Payment = {
  id: string
  couponCode: string
  discount: number
  total: number
  name: string
  updatedAt: string
  status: string
  auctionId: string
  seller: string
  buyer: string
  paymentIntentId: string
  stripeSessionId: string
  userId: string
}

export type Coupon = {
  couponId: number
  couponCode: string
  discountAmount: number
  minAmount: number
}

export type Shipping = {
  shippingId: string
  paymentId: string
  payment: Payment
  trackingCode: string
  trackingUrl: string
  rate: string
  carrier: string
  updatedAt: string
  name: string
  company: string
  street1: string
  street2: string
  city: string
  state: string
  zip: string
  country: string
  email: string
}
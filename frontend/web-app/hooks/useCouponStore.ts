import {Coupon} from "@/types"
import {create} from "zustand"

type State = {
  coupons: Coupon[]
}

type Actions = {
  setCoupons: (coupons: Coupon[]) => void
  addCoupons: (coupons: Coupon[]) => void
}

// @ts-ignore
export const useCouponStore = create<State & Actions>((set) => ({
  coupons: [],
  setCoupon: (coupons: Coupon[]) => {
    set(() => ({
      coupons
    }))
  },
  addCoupon: (coupons: Coupon[]) => {
    set(() => ({
      coupons
    }))
  }
  // setBids: (bids: Bid[]) => {
  //   set(() => ({
  //     bids
  //   }))
  // },
  //
  // addBid: (bid: Bid) => {
  //   set((state) => ({
  //     bids: !state.bids.find(x => x.id === bid.id) ? [bid, ...state.bids] : [...state.bids]
  //   }))
  // },
  //
  // setOpen: (value: boolean) => {
  //   set(() => ({
  //     open: value
  //   }))
  // }
}))
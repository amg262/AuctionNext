import {Coupon} from "@/types"
import {create} from "zustand"

type State = {
  coupons: Coupon[]
}

type Actions = {
  setCoupons: (coupons: Coupon[]) => void
  addCoupons: (coupons: Coupon[]) => void
}

/**
 * Creates a store to manage coupon-related state and actions.
 * Allows setting coupon data and adding coupons.
 */
// @ts-ignore
export const useCouponStore = create<State & Actions>((set) => ({
  coupons: [],

  /**
   * Sets the list of coupons.
   * @param coupons Array of Coupon objects.
   */
  setCoupon: (coupons: Coupon[]) => {
    set(() => ({
      coupons
    }))
  },

  /**
   * Adds new coupons to the existing list.
   * @param coupons Array of new Coupon objects to add.
   */
  addCoupon: (coupons: Coupon[]) => {
    set(() => ({
      coupons
    }))
  }
}))
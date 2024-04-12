import {create} from "zustand"

type State = {
  pageNumber: number
  pageSize: number
  pageCount: number
  searchTerm: string
  searchValue: string
  orderBy: string
  filterBy: string
  seller?: string
  winner?: string
  gridColumns: number
}

type Actions = {
  setParams: (params: Partial<State>) => void
  reset: () => void
  setSearchValue: (value: string) => void
  setGridColumns: (value: number) => void
}

const initialState: State = {
  pageNumber: 1,
  pageSize: 12,
  pageCount: 1,
  searchTerm: '',
  searchValue: '',
  orderBy: 'make',
  filterBy: 'live',
  seller: undefined,
  winner: undefined,
  gridColumns: 3
}

/**
 * Store for managing UI parameters for filtering, sorting, and pagination.
 */
export const useParamsStore = create<State & Actions>()((set) => ({
  ...initialState,

  /**
   * Updates one or more parameters in the store.
   * @param newParams Partial object containing one or more parameters to update.
   */
  setParams: (newParams: Partial<State>) => {
    set((state) => {
      if (newParams.pageNumber) {
        return {...state, pageNumber: newParams.pageNumber}
      } else {
        return {...state, ...newParams, pageNumber: 1}
      }
    })
  },

  /**
   * Resets all parameters to their initial states.
   */
  reset: () => set(initialState),

  /**
   * Sets the search value for filtering.
   * @param value The new search term.
   */
  setSearchValue: (value: string) => {
    set({searchValue: value})
  },

  /**
   * Sets the number of columns in a grid layout.
   * @param value The new number of columns.
   */
  setGridColumns: (value: number) => {
    set({gridColumns: value})
  }
}))
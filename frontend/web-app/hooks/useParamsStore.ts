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

export const useParamsStore = create<State & Actions>()((set) => ({
  ...initialState,

  setParams: (newParams: Partial<State>) => {
    set((state) => {
      if (newParams.pageNumber) {
        return {...state, pageNumber: newParams.pageNumber}
      } else {
        return {...state, ...newParams, pageNumber: 1}
      }
    })
  },

  reset: () => set(initialState),

  setSearchValue: (value: string) => {
    set({searchValue: value})
  },

  setGridColumns: (value: number) => {
    set({gridColumns: value})
  }
}))
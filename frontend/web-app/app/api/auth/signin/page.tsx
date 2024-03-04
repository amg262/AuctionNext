import EmptyFilter from '@/app/components/EmptyFilter'
import React from 'react'

/**
 * React component for displaying a login prompt using an EmptyFilter component.
 * This component is designed to be displayed when a user needs to be logged in to access a certain feature.
 * It shows a custom message and a login button if the user is not authenticated.
 * @param searchParams Parameters passed to the component, including a callback URL.
 * @returns A React component that renders UI elements for the login prompt.
 */
export default function Page({searchParams}: { searchParams: { callbackUrl: string } }) {
  return (
      <EmptyFilter
          title='You need to be logged in to do that'
          subtitle='Please click below to sign in'
          showLogin
          callbackUrl={searchParams.callbackUrl}
      />
  )
}

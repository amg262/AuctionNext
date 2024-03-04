import {getServerSession} from "next-auth";
import {authOptions} from "../api/auth/[...nextauth]/route";
import {getToken} from "next-auth/jwt";
import {cookies, headers} from 'next/headers';
import {NextApiRequest} from "next";

export async function getSession() {
  return await getServerSession(authOptions);
}

/**
 * Retrieves the session information from the server side.
 * This function wraps the getServerSession method from next-auth to obtain the current user session based on authentication tokens.
 * @returns A Promise that resolves to the session object if a session exists, or null if no session is found.
 */
export async function getCurrentUser() {
  try {
    const session = await getSession();

    if (!session) return null;

    return session.user

  } catch (error) {
    return null;
  }
}

/**
 * Attempts to retrieve the current user's session and extract user details.
 * This function demonstrates how to use session management to obtain the current authenticated user's information.
 * @returns A Promise that resolves to the user object if the user is authenticated and a session exists, or null if no session is found or an error occurs.
 */
export async function getTokenWorkaround() {
  const req = {
    headers: Object.fromEntries(headers() as Headers),
    cookies: Object.fromEntries(
        cookies()
            .getAll()
            .map(c => [c.name, c.value])
    )
  } as NextApiRequest;

  return await getToken({req});
}
import {getServerSession} from "next-auth";
import {getToken} from "next-auth/jwt";
import {cookies, headers} from 'next/headers';
import {NextApiRequest} from "next";
import {authOptions} from "@/app/api/auth/[...nextauth]/route";

/**
 * Retrieves the session information using NextAuth configuration.
 * @returns A promise that resolves with the session data, or undefined if no session exists.
 */
export async function getSession() {
  return await getServerSession(authOptions);
}

/**
 * Retrieves the current user's session information and extracts the user details.
 * @returns A promise that resolves with the user object if a session exists, otherwise null.
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
 * A workaround function to manually construct a Next.js API request object to use with getToken for authentication.
 * This function builds a custom request object based on current cookies and headers.
 * @returns A promise that resolves with the JWT token if authentication is successful, otherwise null.
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